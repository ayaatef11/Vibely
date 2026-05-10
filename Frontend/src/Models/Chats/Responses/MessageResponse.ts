export interface MessageResponse{
    id: string
    senderId:string
    content: string
    isEdited:boolean
    isDeleted:boolean
    sentAt: Date
}