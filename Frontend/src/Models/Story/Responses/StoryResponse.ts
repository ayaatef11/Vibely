export interface StoryResponse {
    id: string
    text?: string
    image?: File[]
    video?: File[]
    imageContentType?: string
    userName:string
    isSeen:boolean
    videoContentType?: string
    profileId: string
    createdAt: Date
    hasUnviewedStatus:boolean
}