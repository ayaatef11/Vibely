import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../Environments/environment';
import { DeleteStoryRequest } from '../../../Models/Story/Requests/DeleteStoryRequest';
import { AddCommentRequest } from '../../../Models/Comments/Requests/AddCommentRequest';

@Injectable({
  providedIn: 'root'
})
export class StoryServiceService {
private Url=environment.apiUrl+'Story';
  constructor(private http:HttpClient) { }
  getAll(userId:string){
return this.http.get(`${this.Url}/get-all/${userId}`)
  }
  viewStory(userId:string,storyId:string){
    return this.http.get(`${this.Url}/view/${userId}/${storyId}`)
  }
  getViewers(userId:string,storyId:string){
return this.http.get(`${this.Url}/get-viewers/${userId},${storyId}`)
  }
 addReact(userId: string, storyId: string) {
  return this.http.post(`${this.Url}/add-react/${userId}/${storyId}`, {});
}
  addComment(data: AddCommentRequest){
return this.http.post(`${this.Url}/add-comment`,data)
  }
  addStory(text:string,userId:string){
return this.http.get(`${this.Url}/add/${text},${userId}`)
  }
  getStory(id:string){
return this.http.get(`${this.Url}/get/${id}`)
  }
  deleteStory(data:DeleteStoryRequest){
return this.http.delete(`${this.Url}/delete`,{body:data})

  }
}
