export interface Elevator {
  id: number;
  currentFloor: number;
  direction: number; // 1 for up, -1 for down, 0 for idle
  stopsQueue: number[];
  isMoving: boolean;
}