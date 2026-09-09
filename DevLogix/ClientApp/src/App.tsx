import React, { useEffect, useState, useCallback } from 'react';
import { DragDropContext, DropResult } from '@hello-pangea/dnd';
import { Ticket, TicketStatus, STATUSES } from './types';
import { fetchTickets, updateTicketStatus } from './api';
import KanbanColumn from './components/KanbanColumn';

const App: React.FC = () => {
  const [tickets, setTickets] = useState<Ticket[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Read projectId from the DOM element's data attribute
  const rootElement = document.getElementById('kanban-root');
  const projectId = rootElement ? parseInt(rootElement.dataset.projectId || '0', 10) : 0;

  const loadTickets = useCallback(async () => {
    if (!projectId) {
      setError('No project ID specified');
      setLoading(false);
      return;
    }
    try {
      setLoading(true);
      const data = await fetchTickets(projectId);
      setTickets(data.tickets);
      setError(null);
    } catch (err) {
      setError('Failed to load tickets');
    } finally {
      setLoading(false);
    }
  }, [projectId]);

  useEffect(() => {
    loadTickets();
  }, [loadTickets]);

  const handleDragEnd = async (result: DropResult) => {
    const { destination, source, draggableId } = result;

    if (!destination) return;
    if (destination.droppableId === source.droppableId && destination.index === source.index) return;

    const ticketId = parseInt(draggableId, 10);
    const newStatus = destination.droppableId as TicketStatus;

    // Optimistic update
    setTickets((prev) =>
      prev.map((t) => (t.id === ticketId ? { ...t, status: newStatus } : t))
    );

    try {
      await updateTicketStatus(ticketId, newStatus);
    } catch {
      // Revert on failure
      loadTickets();
      setError('Failed to update status. Reverted.');
    }
  };

  const getTicketsByStatus = (status: TicketStatus): Ticket[] =>
    tickets.filter((t) => t.status === status);

  if (loading) {
    return (
      <div style={{ textAlign: 'center', padding: '40px', color: '#888' }}>
        Loading Kanban board...
      </div>
    );
  }

  if (error) {
    return (
      <div
        style={{
          textAlign: 'center',
          padding: '20px',
          color: '#dc3545',
          backgroundColor: '#f8d7da',
          borderRadius: '6px',
        }}
      >
        {error}
        <button
          onClick={loadTickets}
          style={{
            marginLeft: '12px',
            padding: '4px 12px',
            border: '1px solid #dc3545',
            borderRadius: '4px',
            backgroundColor: 'transparent',
            color: '#dc3545',
            cursor: 'pointer',
          }}
        >
          Retry
        </button>
      </div>
    );
  }

  return (
    <div>
      <DragDropContext onDragEnd={handleDragEnd}>
        <div
          style={{
            display: 'flex',
            gap: '12px',
            overflowX: 'auto',
            padding: '8px 0',
          }}
        >
          {STATUSES.map((status) => (
            <KanbanColumn key={status} status={status} tickets={getTicketsByStatus(status)} />
          ))}
        </div>
      </DragDropContext>
    </div>
  );
};

export default App;
