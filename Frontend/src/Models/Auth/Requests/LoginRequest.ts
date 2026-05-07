export interface LoginRequest {
  userName: string
  password: string
  timeOutInMinutes?: number |null,
  isLoginNotificationsEnabled:boolean
}