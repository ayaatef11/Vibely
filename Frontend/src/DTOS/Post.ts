import { CommentResponse } from "../Models/Posts/Responses/CommentResponse"

export interface Post{
     id: string
        reactsCount:number
        shareCount: number
        commentsCount: number
        createdAt: Date
        feelingState: number
        title: string
        text: string
        mediaUrls: string[]
        isHidden: boolean
        saverIds: string[]
        isSaved: boolean
        isReel: boolean
        isLiked:boolean
        showComments:boolean
        userName:string
        comments:CommentResponse[]
}