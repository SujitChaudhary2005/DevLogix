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
            padding: '16px',
            marginBottom: '12px',
            backgroundColor: snapshot.isDragging ? '#ffffff' : '#ffffff',
            borderRadius: '12px',
            boxShadow: snapshot.isDragging
              ? '0 20px 25px -5px rgba(0, 0, 0, 0.1), 0 10px 10px -5px rgba(0, 0, 0, 0.04)'
              : '0 1px 3px 0 rgba(0, 0, 0, 0.1), 0 1px 2px 0 rgba(0, 0, 0, 0.06)',
            border: snapshot.isDragging ? '1px solid #6366f1' : '1px solid #e2e8f0',
            cursor: 'grab',
            transition: 'box-shadow 0.2s ease, border-color 0.2s ease',
            transform: snapshot.isDragging ? 'rotate(2deg) scale(1.02)' : 'none',
          }}
        >
          <div style={{ fontWeight: 600, fontSize: '14px', color: '#0f172a', marginBottom: '10px', lineHeight: '1.4' }}>
            {ticket.title}
          </div>
          <div style={{ display: 'flex', gap: '8px', flexWrap: 'wrap', alignItems: 'center' }}>
            <span
              style={{
                padding: '4px 10px',
                borderRadius: '999px',
                fontSize: '11px',
                fontWeight: 600,
                backgroundColor: PRIORITY_COLORS[ticket.priority] + '15',
                color: PRIORITY_COLORS[ticket.priority],
                border: `1px solid ${PRIORITY_COLORS[ticket.priority]}30`,
              }}
            >
              {ticket.priority}
            </span>
            {ticket.assignedToName && (
              <span
                style={{
                  padding: '4px 10px',
                  borderRadius: '999px',
                  fontSize: '11px',
                  fontWeight: 500,
                  backgroundColor: '#f1f5f9',
                  color: '#475569',
                  border: '1px solid #e2e8f0'
                }}
              >
                <i className="bi bi-person-fill me-1"></i>
                {ticket.assignedToName}
              </span>
            )}
          </div>
          
          <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginTop: '12px', paddingTop: '10px', borderTop: '1px solid #f1f5f9' }}>
            <div style={{ fontSize: '11px', color: '#94a3b8', fontWeight: 500 }}>
              #{ticket.id}
            </div>
            {ticket.commentCount > 0 && (
              <div style={{ fontSize: '12px', color: '#64748b', display: 'flex', alignItems: 'center', gap: '4px' }}>
                <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" fill="currentColor" viewBox="0 0 16 16">
                  <path d="M2.678 11.894a1 1 0 0 1 .287.801 10.97 10.97 0 0 1-.398 2c1.395-.323 2.247-.697 2.634-.893a1 1 0 0 1 .71-.074A8.06 8.06 0 0 0 8 14c3.996 0 7-2.807 7-6 0-3.192-3.004-6-7-6S1 4.808 1 8c0 1.468.617 2.83 1.678 3.894zm-.493 3.905a21.682 21.682 0 0 1-.713.129c-.2.032-.352-.176-.273-.362a9.68 9.68 0 0 0 .244-.637l.003-.01c.248-.72.35-1.548.31-2.316-1.12-1.1-1.892-2.5-1.892-4.004C.5 4.027 3.862 1 8 1s7.5 3.027 7.5 7c0 3.972-3.362 7-7.5 7-1.12 0-2.19-.24-3.15-.658a1.5 1.5 0 0 0-.66.075c-.9.2-2.17.54-3.693.89z"/>
                </svg>
                {ticket.commentCount}
              </div>
            )}
          </div>
        </div>
      )}
    </Draggable>
  );
};

export default KanbanCard;
