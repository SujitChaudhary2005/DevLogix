import React from 'react';
import { Droppable } from '@hello-pangea/dnd';
import { Ticket, TicketStatus, STATUS_COLORS } from '../types';
import KanbanCard from './KanbanCard';

interface KanbanColumnProps {
  status: TicketStatus;
  tickets: Ticket[];
}

const KanbanColumn: React.FC<KanbanColumnProps> = ({ status, tickets }) => {
  const displayName = status === 'InProgress' ? 'In Progress' : status;

  return (
    <div
      style={{
        flex: '1 1 0',
        minWidth: '220px',
        maxWidth: '300px',
        display: 'flex',
        flexDirection: 'column',
      }}
    >
      <div
        style={{
          padding: '10px 14px',
          fontWeight: 700,
          fontSize: '13px',
          textTransform: 'uppercase',
          letterSpacing: '0.05em',
          color: STATUS_COLORS[status],
          borderBottom: `3px solid ${STATUS_COLORS[status]}`,
          marginBottom: '8px',
          display: 'flex',
          justifyContent: 'space-between',
          alignItems: 'center',
        }}
      >
        <span>{displayName}</span>
        <span
          style={{
            backgroundColor: STATUS_COLORS[status] + '20',
            color: STATUS_COLORS[status],
            padding: '2px 8px',
            borderRadius: '12px',
            fontSize: '12px',
            fontWeight: 600,
          }}
        >
          {tickets.length}
        </span>
      </div>

      <Droppable droppableId={status}>
        {(provided, snapshot) => (
          <div
            ref={provided.innerRef}
            {...provided.droppableProps}
            style={{
              flex: 1,
              padding: '4px',
              minHeight: '200px',
              backgroundColor: snapshot.isDraggingOver ? '#f0f7ff' : '#f8f9fa',
              borderRadius: '6px',
              transition: 'background-color 0.2s ease',
            }}
          >
            {tickets.map((ticket, index) => (
              <KanbanCard key={ticket.id} ticket={ticket} index={index} />
            ))}
            {provided.placeholder}
          </div>
        )}
      </Droppable>
    </div>
  );
};

export default KanbanColumn;
