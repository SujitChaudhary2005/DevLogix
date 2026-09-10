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
        minWidth: '260px',
        maxWidth: '320px',
        display: 'flex',
        flexDirection: 'column',
        backgroundColor: '#f8fafc',
        borderRadius: '12px',
        padding: '16px',
        border: '1px solid #e2e8f0'
      }}
    >
      <div
        style={{
          paddingBottom: '12px',
          fontWeight: 600,
          fontSize: '14px',
          color: '#1e293b',
          display: 'flex',
          justifyContent: 'space-between',
          alignItems: 'center',
          marginBottom: '8px',
        }}
      >
        <div style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
          <div style={{ width: '8px', height: '8px', borderRadius: '50%', backgroundColor: STATUS_COLORS[status] }}></div>
          <span>{displayName}</span>
        </div>
        <span
          style={{
            backgroundColor: '#e2e8f0',
            color: '#475569',
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
              minHeight: '200px',
              backgroundColor: snapshot.isDraggingOver ? '#eef2ff' : 'transparent',
              borderRadius: '8px',
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
