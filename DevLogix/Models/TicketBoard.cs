using System;
using System.Collections.Generic;
using System.Linq;

namespace DevLogix.Models;

public class TicketBoard
{
    private readonly Dictionary<TicketStatus, List<Ticket>> _columns = new();

    public TicketBoard(IEnumerable<Ticket> tickets)
    {
        foreach (var status in Enum.GetValues<TicketStatus>())
            _columns[status] = new List<Ticket>();
        foreach (var ticket in tickets)
            _columns[ticket.Status].Add(ticket);
    }

    // Indexer — board[TicketStatus.InProgress]
    public IReadOnlyList<Ticket> this[TicketStatus status] => _columns[status].AsReadOnly();

    public int TotalCount => _columns.Values.Sum(col => col.Count);

    public IEnumerable<TicketStatus> Statuses => Enum.GetValues<TicketStatus>();
}
