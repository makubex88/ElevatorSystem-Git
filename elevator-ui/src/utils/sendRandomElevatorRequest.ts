// src/utils/randomRequest.ts
import { sendElevatorRequest, type Direction } from "../api/elevatorApi";

const floors = Array.from({ length: 10 }, (_, i) => i + 1);

export function getRandomFloorAndDirection(): { floor: number; direction: Direction } {
  const floor = floors[Math.floor(Math.random() * floors.length)];
  if (floor === 1) return { floor, direction: 'up' };
  if (floor === 10) return { floor, direction: 'down' };
  return { floor, direction: Math.random() > 0.5 ? 'up' : 'down' };
}

export async function sendRandomElevatorRequest(): Promise<{
  floor: number;
  direction: Direction;
  assignedElevator: number;
}> {
  const { floor, direction } = getRandomFloorAndDirection();
  const assignedElevator = await sendElevatorRequest(floor, direction);
  return { floor, direction, assignedElevator };
}
