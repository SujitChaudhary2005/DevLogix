export type TicketStatus = 'Todo' | 'InProgress' | 'Resolved' | 'Closed' | 'Reopened';
export type Priority = 'Low' | 'Medium' | 'High' | 'Critical';

export interface Ticket {
  id: number;
  title: string;
  status: TicketStatus;
  priority: Priority;
  createdByName: string;
  assignedToName: string | null;
  createdAt: string;
  commentCount: number;
}

export interface TicketListResponse {
  tickets: Ticket[];
  statusFilter: TicketStatus | null;
  priorityFilter: Priority | null;
  assigneeFilter: string | null;
  projectId: number;
  projectTitle: string;
  currentPage: number;
  totalPages: number;
  pageSize: number;
}

export const STATUSES: TicketStatus[] = ['Todo', 'InProgress', 'Resolved', 'Closed', 'Reopened'];

export const STATUS_COLORS: Record<TicketStatus, string> = {
  Todo: '#6c757d',
  InProgress: '#0d6efd',
  Resolved: '#198754',
  Closed: '#212529',
  Reopened: '#ffc107',
};

export const PRIORITY_COLORS: Record<Priority, string> = {
  Low: '#0dcaf0',
  Medium: '#ffc107',
  High: '#dc3545',
  Critical: '#dc3545',
};
