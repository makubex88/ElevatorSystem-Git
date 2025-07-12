import React from 'react';
import styled from 'styled-components';
import type { Elevator } from '../types/Elevator';
import { ElevatorStatusCard } from './ElevatorStatusCard';

const Row = styled.div`
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
`;

type Props = {
  elevators: Elevator[];
};

export const ElevatorStatusList: React.FC<Props> = ({ elevators }) => {
  return (
    <Row>
      {elevators.map(elevator => (
        <ElevatorStatusCard key={elevator.id} elevator={elevator} />
      ))}
    </Row>
  );
};
