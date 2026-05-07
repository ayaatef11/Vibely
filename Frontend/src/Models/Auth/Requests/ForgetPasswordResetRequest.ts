export interface ForgetPasswordResetRequest {
    email: string
    code: string
    newPassword: string
    timeOutInMinutes?: number | null
}