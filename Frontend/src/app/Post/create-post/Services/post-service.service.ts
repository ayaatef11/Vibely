import { Injectable } from '@angular/core';
import { environment } from '../../../../Environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { AddPostRequest } from '../../../../Models/Posts/Requests/AddPostRequest';
import { EditPostRequest } from '../../../../Models/Posts/Requests/EditPostRequest';
import { catchError, throwError } from 'rxjs';
import { StartSharePostRequest } from '../../../../Models/Posts/Requests/StartSharePostRequest';
import { RevokeSharePostRequest } from '../../../../Models/Posts/Requests/RevokeSharePostRequest';
import { PostResponse } from '../../../../Models/Posts/Responses/PostResponse';

@Injectable({
  providedIn: 'root'
})
export class PostServiceService {
  private Url = environment.apiUrl + 'post';

  constructor(private http: HttpClient) { }

  addPost(data: AddPostRequest) {
    let params = new HttpParams()
      .set('FeelingState', data.feelingState)
      .set('Title', data.title)
      .set('Text', data.text)
      .set('ProfileId', data.ProfileId);

    const formData = new FormData();

    // data.mediaFiles.forEach(file => {
    //   formData.append('Media', file);
    // });

    return this.http.post<PostResponse>(`${this.Url}/add`, formData, { params });
  }

  editPost(data: EditPostRequest) {
    return this.http.put(`${this.Url}/edit`, data);
  }

  getUserPosts(profileId: string) {
    let params = new HttpParams()
      .set('profileId', profileId)
    return this.http.get(`${this.Url}/user`, { params });
  }

  getPost(postId: string) {
    return this.http.get<PostResponse>(`${this.Url}/${postId}`)
  }

  deletePost(id: string) {
    return this.http.delete(`${this.Url}/${id}`);
  }

  getAllPosts(userId: string) {
    let params = new HttpParams().set('userId', userId)
    return this.http.get<PostResponse[]>(`${this.Url}/get-all`, { params });
  }

  searchPosts(keyword: string) {
    let params = new HttpParams()
      .set('keyword', keyword)
    return this.http.get(`${this.Url}/show-posts`, { params })
    ;
  }

  getTrendingPosts() {
    return this.http.get(`${this.Url}/trending-posts`);
  }

  getSharesCount(postId: string) {
    let params = new HttpParams()
      .set('postId', postId)
    return this.http.get(`${this.Url}/shares-count`, { params });
  }

  getLikesCount(postId: string) {
    let params = new HttpParams()
      .set('postId', postId)
    return this.http.get(`${this.Url}/likes-count`, { params });
  }

  hidePost(postId: string) {
    let params = new HttpParams().set('postId', postId)
    return this.http.get(`${this.Url}/hide-post`, { params });
  }


}
