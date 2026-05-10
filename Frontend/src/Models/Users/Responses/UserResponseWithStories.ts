import { File } from "buffer"
import { StoryResponse } from "../../Story/Responses/StoryResponse"

export interface UserResponseWithStories {
    fullName: string
    location: string
    profileImage: File[]
    stories?: StoryResponse[]
}