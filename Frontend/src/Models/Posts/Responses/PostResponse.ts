import { CommentResponse } from "./CommentResponse"

export interface PostResponse{
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
    profileId:string
    comments:CommentResponse[] 
    showComments:boolean
    userName:string
    showCommentBox:boolean
    newCommentText:string
    showShareBox:boolean
    shareLink: string
    shareLinkLoading:boolean
    showMenu:boolean
}