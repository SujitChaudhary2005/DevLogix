// DevLogix — Site JavaScript

// jQuery AJAX: Inline comment posting on ticket details
$(document).ready(function () {
    // AJAX comment submission
    $('#ajax-comment-form').on('submit', function (e) {
        e.preventDefault();
        var form = $(this);
        var url = form.attr('action');
        var content = form.find('textarea[name="content"]').val();
        var ticketId = form.find('input[name="ticketId"]').val();

        if (!content || content.trim() === '') return;

        $.ajax({
            url: '/api/tickets/' + ticketId + '/comments',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ content: content }),
            success: function () {
                location.reload(); // Simple reload to show new comment
            },
            error: function () {
                alert('Failed to post comment. Please try again.');
            }
        });
    });

    // AJAX status dropdown change
    $('#ajax-status-select').on('change', function () {
        var ticketId = $(this).data('ticket-id');
        var newStatus = $(this).val();

        $.ajax({
            url: '/api/tickets/' + ticketId + '/status',
            method: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify({ newStatus: newStatus }),
            success: function () {
                location.reload();
            },
            error: function () {
                alert('Failed to update status.');
            }
        });
    });

    // Delete confirmation
    $('[data-confirm]').on('click', function (e) {
        if (!confirm($(this).data('confirm'))) {
            e.preventDefault();
        }
    });

    // Auto-dismiss alerts after 5 seconds
    setTimeout(function () {
        $('.alert-dismissible').alert('close');
    }, 5000);
});
