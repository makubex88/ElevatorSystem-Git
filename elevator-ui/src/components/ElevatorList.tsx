// src/components/ElevatorList.tsx

import styled from "styled-components";
import ElevatorCard from "./ElevatorCard";
import type { Elevator } from "../types/Elevator";

const ElevatorContainer = styled.div`
  display: flex;
  gap: 20px;          // space between cards
  justify-content: center;  // center horizontally
  flex-wrap: nowrap;  // no wrap, scroll horizontally if overflow
  overflow-x: auto;
  padding: 1rem;
`;

interface ElevatorListProps {
  elevators: Elevator[];
}

export default function ElevatorList({ elevators }: ElevatorListProps) {
  return (
    <ElevatorContainer>
      {elevators.map((elevator) => (
        <ElevatorCard key={elevator.id} elevator={elevator} />
      ))}
    </ElevatorContainer>
  );
}
