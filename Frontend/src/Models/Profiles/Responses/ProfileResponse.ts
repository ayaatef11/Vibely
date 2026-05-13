import { PostResponse } from "../../Posts/Responses/PostResponse"

export interface ProfileResponse {
    id: string
    postCount: number
    followerCount: number
    followingCount: number
    bio: string
    fullName: string
    userName: string
    website: string
    location: string
    userId:string
    profileImage: string
    backgroundImage: string
    profileImageContentType: string
    backgroundImageContentType: string
    isFollowed:boolean
    isRequested: boolean
    posts:PostResponse[]
}