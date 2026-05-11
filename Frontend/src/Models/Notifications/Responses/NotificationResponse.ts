export interface NotificationResponse {
    id: string;
    senderId: string;
    senderName: string;
    senderProfilePicture: string;
    type: 'NewMessage' | 'NewPost' | 'FriendRequest' | 'FriendRequestAccepted' | 'Like' | 'Comment';
    message: string;
    referenceId?: string;
    isRead: boolean;
    createdAt: string;
}