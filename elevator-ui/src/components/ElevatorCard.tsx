import { styled } from "styled-components";
import type { Elevator } from "../types/Elevator";

const FLOOR_HEIGHT = 40; // px height per floor

const Card = styled.div`
  border: 1px solid #ccc;
  padding: 1rem;
  border-radius: 12px;
  margin: 1rem 0;
  background: #f8f8f8;
  width: 120px;
`;

const ElevatorShaft = styled.div`
  position: relative;
  height: ${FLOOR_HEIGHT * 10}px; // 10 floors
  width: 50px;
  border: 2px solid #333;
  margin: 0 auto;
  background: #ddd;
`;

const ElevatorCar = styled.div<{ floor: number }>`
  position: absolute;
  width: 46px;
  height: ${FLOOR_HEIGHT - 4}px;
  background: #0077cc;
  border-radius: 8px;
  bottom: ${(props) => (props.floor - 1) * FLOOR_HEIGHT}px;
  transition: bottom 2s ease-in-out;
  color: white;
  text-align: center;
  line-height: ${FLOOR_HEIGHT - 4}px;
  font-weight: bold; 
`;

const CustomH3 = styled.h3`color: black;
    text-align: center; `;

const CustomDiv = styled.div`
    color: black;
}`;

export default function ElevatorCard({ elevator }: { elevator: Elevator }) {
  return (
    <Card>
      <CustomH3>Elevator #{elevator.id}</CustomH3>
      <ElevatorShaft>
        <ElevatorCar floor={elevator.currentFloor}>{elevator.currentFloor}</ElevatorCar>
      </ElevatorShaft>
      <CustomDiv>
        <p>Direction: {elevator.direction}</p>
            <p>Stops: {elevator.stopsQueue.join(", ") || "None"}</p>
      </CustomDiv>
     
    </Card>
  );
}
