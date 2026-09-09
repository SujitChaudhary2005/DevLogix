import React from 'react';
import { Draggable } from '@hello-pangea/dnd';
import { Ticket, PRIORITY_COLORS } from '../types';

interface KanbanCardProps {
  ticket: Ticket;
  index: number;
}

const KanbanCard: React.FC<KanbanCardProps> = ({ ticket, index }) => {
  return (
    <Draggable draggableId={String(ticket.id)} index={index}>
      {(provided, snapshot) => (
        <div
          ref={provided.innerRef}
          {...provided.draggableProps}
          {...provided.dragHandleProps}
          style={{
            ...provided.draggableProps.style,
            padding: '12px',
            marginBottom: '8px',
            backgroundColor: snapshot.isDragging ? '#e3f2fd' : '#fff',
            borderRadius: '6px',
            boxShadow: snapshot.isDragging
              ? '0 4px 12px rgba(0,0,0,0.15)'
              : '0 1px 3px rgba(0,0,0,0.08)',
            border: '1px solid #e0e0e0',
            cursor: 'grab',
          }}
        >
          <div style={{ fontWeight: 600, fontSize: '14px', marginBottom: '6px' }}>
            {ticket.title}
          </div>
          <div style={{ display: 'flex', gap: '6px', flexWrap: 'wrap' }}>
            <span
              style={{
                padding: '2px 8px',
                borderRadius: '12px',
                fontSize: '11px',
                fontWeight: 600,
                backgroundColor: PRIORITY_COLORS[ticket.priority] + '20',
                color: PRIORITY_COLORS[ticket.priority],
                border: `1px solid ${PRIORITY_COLORS[ticket.priority]}40`,
              }}
            >
              {ticket.priority}
            </span>
            {ticket.assignedToName && (
              <span
                style={{
                  padding: '2px 8px',
                  borderRadius: '12px',
                  fontSize: '11px',
                  backgroundColor: '#f0f0f0',
                  color: '#555',
                }}
              >
                {ticket.assignedToName}
              </span>
            )}
          </div>
          {ticket.commentCount > 0 && (
            <div style={{ fontSize: '12px', color: '#888', marginTop: '6px' }}>
              💬 {ticket.commentCount}
            </div>
          )}
        </div>
      )}
    </Draggable>
  );
};

export default KanbanCard;
