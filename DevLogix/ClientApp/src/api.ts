import { Ticket, TicketStatus, TicketListResponse } from './types';

const API_BASE = '/api/tickets';

export async function fetchTickets(projectId: number): Promise<TicketListResponse> {
  const response = await fetch(`${API_BASE}?projectId=${projectId}&pageSize=100`);
  if (!response.ok) throw new Error('Failed to fetch tickets');
  return response.json();
}

export async function updateTicketStatus(ticketId: number, newStatus: TicketStatus): Promise<void> {
  const response = await fetch(`${API_BASE}/${ticketId}/status`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ newStatus }),
  });
  if (!response.ok) throw new Error('Failed to update ticket status');
}
