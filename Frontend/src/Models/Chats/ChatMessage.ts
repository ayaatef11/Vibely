export interface ChatMessage {
  senderId: string;
  receiverId: string;
  content: string;
  sentAt: Date;
}