import { useEffect, useRef, useState } from "react";
import styled from "styled-components";
import { getAllElevators } from "./api/elevatorApi";
import type { Elevator } from "./types/Elevator";
import { HubConnectionBuilder, HubConnection, HttpTransportType } from "@microsoft/signalr";
import ElevatorList from "./components/ElevatorList";
import { ElevatorRequestLog } from './components/ElevatorRequestLog';
import { sendRandomElevatorRequest } from "./utils/sendRandomElevatorRequest";
import { ElevatorStatusList } from "./components/ElevatorStatusList";

const Container = styled.div`
  padding: 2rem;
  max-width: 1000px;
  margin: auto;
`;

const Header = styled.h1`
  text-align: center;
`;

type ElevatorRequest = {
  id: number;
  floor: number;
  direction: 'up' | 'down';
  elevatorId: number;
  elevatorCurrentFloor: number;
};

function App() {
  const [elevators, setElevators] = useState<Elevator[]>([]);
  const elevatorsRef = useRef<Elevator[]>([]);
  const [connection, setConnection] = useState<HubConnection | null>(null);
  const [requestLog, setRequestLog] = useState<ElevatorRequest[]>([]);

  // Initial fetch only
  useEffect(() => {
    (async () => {
      try {
        const res = await getAllElevators();
        setElevators(res.data);
      } catch (error) {
        console.error("Failed to fetch elevators", error);
      }
    })();
  }, []);

   // Keep ref in sync with state
  useEffect(() => {
    elevatorsRef.current = elevators;
  }, [elevators]);

  // Setup SignalR connection once
  useEffect(() => {
    const newConnection = new HubConnectionBuilder()
      .withUrl("https://localhost:7232/elevatorHub", {
        transport: HttpTransportType.WebSockets,
        withCredentials: true,
      })
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);

    return () => {
      newConnection.stop();
    };
  }, []);

  // Start SignalR connection and subscribe to updates
  useEffect(() => {
    if (!connection) return;

    connection.start()
      .then(() => {
        console.log("Connected to SignalR");
        console.log("connection", connection);


        connection.on("ReceiveElevatorUpdate", (updatedElevator: Elevator) => {
          console.log("Current updatedElevator:", updatedElevator);
          setElevators(prev =>
            prev.map(e => e.id === updatedElevator.id ? updatedElevator : e)
          );
        });
      })
      .catch(e => console.error("SignalR connection error:", e));

    // Clean up listener on unmount or connection change
    return () => {
      connection.off("ReceiveElevatorUpdate");
    };
  }, [connection]);

  useEffect(() => {
    const interval = setInterval(async () => {
      try {
        const { floor, direction, assignedElevator } = await sendRandomElevatorRequest();

        const currentElevators = elevatorsRef.current;

        if (currentElevators.length === 0) return;

        const logEntry: ElevatorRequest = {
          id: Date.now(),
          floor,
          direction,
          elevatorId: assignedElevator,
          elevatorCurrentFloor: 0, // Placeholder, can be updated with actual current floor if needed
        };

        setRequestLog(prev => [logEntry, ...prev]);
      } catch (error) {
        console.error("Error sending random elevator request:", error);
      }
    }, 10000);

    return () => clearInterval(interval);
  }, []); // Empty dependency array — interval set once

  return (
    <Container>
      <Header>Elevator Control System</Header>
      <ElevatorStatusList elevators={elevators} />
      <ElevatorRequestLog requests={requestLog} />
      <ElevatorList elevators={elevators} />
    </Container>
  );
}

export default App;
