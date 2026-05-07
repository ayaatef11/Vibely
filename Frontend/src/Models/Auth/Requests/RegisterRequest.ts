export interface RegisterRequest{
  userName: string
  fullName: string
  email: string
  password: string
  confirmPassword:string
  location: string  
  timeOutInMinutes?:number | null
}




