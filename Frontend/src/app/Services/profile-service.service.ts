import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../Environments/environment';
import { EditProfileRequest } from '../../Models/Profiles/Requests/EditProfileRequest';
import { ProfileResponse } from '../../Models/Profiles/Responses/ProfileResponse';
import { UserResponse } from '../../Models/Users/Responses/UserResponse';

@Injectable({
  providedIn: 'root'
})
export class ProfileServiceService {
  private Url = environment.apiUrl + 'Profile';
  constructor(private http: HttpClient) { }

  editProfile(data: EditProfileRequest) {
const formData = new FormData();

  formData.append('UserId', data.userId);
  formData.append('Bio', data.bio);
  formData.append('Website', data.website);
  formData.append('FullName', data.fullName);
  formData.append('UserName', data.userName);
  formData.append('Location', data.location);

if (!data.profileImage || data.profileImage.length === 0) {
  formData.append('ProfileImage', new Blob());
}

if (!data.backgroundImage || data.backgroundImage.length === 0) {
  formData.append('BackgroundImage', new Blob());
}

  return this.http.put<ProfileResponse>(`${this.Url}/edit`, formData);
  }

  getFollowers(userId: string) {
    var params = new HttpParams().set('userId', userId);
    return this.http.get<UserResponse[]>(`${this.Url}/followers`, { params })
  }

  getFollowing(userId: string) {
    var params = new HttpParams().set('userId', userId);
    return this.http.get<UserResponse[]>(`${this.Url}/following`, { params })
  }

  viewProfile(profileId: string) {
    var params = new HttpParams().set('profileId', profileId);
    return this.http.get<ProfileResponse>(`${this.Url}/view`, { params })
  }

}
