import React from 'react';
import styled from 'styled-components';
import type { Elevator } from '../types/Elevator';

const Card = styled.div`
  border: 1px solid #ccc;
  border-radius: 10px;
  padding: 1rem;
  width: 200px;
  margin: 0.5rem;
  box-shadow: 0 0 6px rgba(0, 0, 0, 0.1);
`;

const Title = styled.h3`
  margin: 0 0 0.5rem 0;
`;

const Item = styled.p`
  margin: 0.25rem 0;
`;

const getDirectionLabel = (dir: number) => {
  switch (dir) {
    case 1:
      return '⬆️ Up';
    case -1:
      return '⬇️ Down';
    default:
      return '⏸ Idle';
  }
};

type Props = {
  elevator: Elevator;
};

export const ElevatorStatusCard: React.FC<Props> = ({ elevator }) => {
  return (
    <Card>
      <Title>Elevator #{elevator.id}</Title>
      <Item><strong>Floor:</strong> {elevator.currentFloor}</Item>
      <Item><strong>Direction:</strong> {getDirectionLabel(elevator.direction)}</Item>
      <Item><strong>Moving:</strong> {elevator.isMoving ? '✅ Yes' : '❌ No'}</Item>
      <Item><strong>Stops:</strong> {elevator.stopsQueue.join(', ') || 'None'}</Item>
    </Card>
  );
};
