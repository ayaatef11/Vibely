import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core'; 
import { Observable } from 'rxjs';
import { environment } from '../../Environments/environment';
import { AcceptFollowRequest } from '../../Models/Follow/Requests/AcceptFollowRequest';
import { FollowRequest } from '../../Models/Follow/Requests/FollowRequest';
import { RejectFollowRequest } from '../../Models/Follow/Requests/RejectFollowRequest';
import { UnFollowRequest } from '../../Models/Follow/Requests/UnFollowRequest';
import { ProfileResponse } from '../../Models/Profiles/Responses/ProfileResponse';
import { UserResponse } from '../../Models/Users/Responses/UserResponse';
import { UserResponseWithStories } from '../../Models/Users/Responses/UserResponseWithStories';

@Injectable({
  providedIn: 'root'
})
export class FollowSerivceService {
  private Url = environment.apiUrl + 'Follow';
  constructor(private http: HttpClient) { }

  acceptFollow(data: AcceptFollowRequest) {
    return this.http.put(`${this.Url}/accept`, data)
  }

  requestFollow(data: FollowRequest): Observable<ProfileResponse> {
    return this.http.post<ProfileResponse>(`${this.Url}/request`, data)
  }

  unrequestFollow(data: FollowRequest): Observable<ProfileResponse> {
    return this.http.post<ProfileResponse>(`${this.Url}/unrequest`, data)
  }

  rejectFollow(data: RejectFollowRequest): Observable<ProfileResponse> {
    return this.http.put<ProfileResponse>(`${this.Url}/reject`, data)
  }

  unfollow(data: UnFollowRequest): Observable<ProfileResponse> {
    return this.http.delete<ProfileResponse>(`${this.Url}/unfollow`, { body: data })
  }

  getFollow(profileId: string): Observable<UserResponse> {
    return this.http.get<UserResponse>(`${this.Url}/Get/${profileId}`)
  }

  FindPeople(profileId: string) {
    return this.http.get<ProfileResponse[]>(`${this.Url}/find-people?profileId=${profileId}`)
    
  }

  getFollowingWithStories(id: string): Observable<UserResponseWithStories[]> {
    return this.http.get<UserResponseWithStories[]>(`${this.Url}/Get-following/${id}`)
  }

  viewRequests(profileId: string): Observable<ProfileResponse[]> {
    return this.http.get<ProfileResponse[]>(`${this.Url}/view?profileId=${profileId}`)
  }
}
