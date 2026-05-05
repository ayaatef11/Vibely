import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../Environments/environment';
import { SavePostRequest } from '../../../../Models/Posts/Requests/SavePostRequest';
import { UnSavePostRequest } from '../../../../Models/Posts/Requests/UnSavePostRequest';
import { PostResponse } from '../../../../Models/Posts/Responses/PostResponse';

@Injectable({
  providedIn: 'root'
})
export class SavedPostsServiceService {
private Url=environment.apiUrl+'SavePost';
  constructor(private http:HttpClient) { }
  savePost(data:SavePostRequest){
return this.http.post(`${this.Url}/Save`,data);
  }

  getSavedPosts(userId:string){
return this.http.get<PostResponse[]>(`${this.Url}/Get/${userId}`);
  }

  unSavePost(data:UnSavePostRequest){
return this.http.delete(`${this.Url}/UnSave`,{body:data})
  }

}
