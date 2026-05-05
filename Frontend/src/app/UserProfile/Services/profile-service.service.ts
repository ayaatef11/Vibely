import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../Environments/environment';
import { EditProfileRequest } from '../../../Models/Profiles/Requests/EditProfileRequest';

@Injectable({
  providedIn: 'root'
})
export class ProfileServiceService {
private Url=environment.apiUrl+'Profile';
  constructor(private http:HttpClient) { }
  editProfile(data:EditProfileRequest){
let params=new HttpParams();
params.set('userId',data.userId)
.set('Bio',data.bio)
.set('website',data.website)
.set('fullName',data.fullName)
.set('userName',data.userName)
.set('location',data.location)

 const formData = new FormData();

    data.backgroundImage.forEach(file => {
      formData.append('Media', file);
    });
      data.profileImage.forEach(file => {
      formData.append('Media', file);
    });
    return this.http.put(`${this.Url}/edit`,formData,{params})
  }
  getFollowers(userId:string){
    var params=new HttpParams().set('userId',userId);
return this.http.get(`${this.Url}/followers`,{params})
  }
  getFollowing(userId:string){
    var params=new HttpParams().set('userId',userId);
return this.http.get(`${this.Url}/following`,{params})
  }
  viewProfile(userId:string){
    var params=new HttpParams().set('userId',userId);
return this.http.get(`${this.Url}/view`,{params})
  }

}
