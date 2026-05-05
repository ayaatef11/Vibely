export interface CommentResponse {
   id: string
   text: string
   reactCount: number
   addedAt: Date
   postId: string
   profileId: string
   profileImage: string
   userName: string
   isLiked: boolean
}