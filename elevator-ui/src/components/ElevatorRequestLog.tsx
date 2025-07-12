// src/components/ElevatorRequestLog.tsx
import React from 'react';

type ElevatorRequest = {
  id: number;
  floor: number;
  direction: 'up' | 'down';
  elevatorId: number;
  elevatorCurrentFloor: number;
};

type Props = {
  requests: ElevatorRequest[];
};

export const ElevatorRequestLog: React.FC<Props> = ({ requests }) => {
  return (
    <div style={{ marginTop: '20px', maxHeight: '150px', overflowY: 'scroll' }}>
      <h2>Request Log</h2>
      <ul style={{ listStyle: 'none', padding: 0 }}>
        {requests
          .slice()
          .reverse()
          .map((req) => (
            <li key={req.id}>
              Floor {req.floor} requesting: {req.direction}, assigned Elevator: {req.elevatorId}
            </li>
        ))}
      </ul>
    </div>
  );
};
