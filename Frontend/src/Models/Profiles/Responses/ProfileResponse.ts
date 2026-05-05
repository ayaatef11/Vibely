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
    socialMediaUserId:string
    profileImage: string
    backgroundImage: string
    profileImageContentType: string
    backgroundImageContentType: string
    posts:PostResponse[]
}