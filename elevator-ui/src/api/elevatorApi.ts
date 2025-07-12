import axios from "axios";

const api = axios.create({
  baseURL: "https://localhost:7232/api/elevator",
});

export const getAllElevators = () => api.get("/");
export const requestElevator = (floor: number, direction: "Up" | "Down") =>
  api.post("/request", { floor, direction });



// src/api/elevatorApi.ts
export type Direction = 'up' | 'down';

export async function sendElevatorRequest(floor: number, direction: Direction) {
  try {
    const response = await fetch('https://localhost:7232/api/elevator/request', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ floor, direction }),
    });

    if (!response.ok) {
      console.error('Failed to send elevator request');
      return null;
    }

    const result = await response.json(); // Expecting: { message: "...", elevatorId: number }
    return result.elevatorId; // Return the assigned elevator ID
  } catch (error) {
    console.error('Error sending elevator request:', error);
    return null;
  }
}