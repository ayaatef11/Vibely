import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs'; 
import { environment } from '../../Environments/environment';
import { AddPostRequest } from '../../Models/Posts/Requests/AddPostRequest';
import { EditPostRequest } from '../../Models/Posts/Requests/EditPostRequest';
import { PostResponse } from '../../Models/Posts/Responses/PostResponse';

@Injectable({
  providedIn: 'root'
})
export class PostServiceService {
  private Url = environment.apiUrl + 'post';

  constructor(private http: HttpClient) { }

  addPost(data: AddPostRequest): Observable<PostResponse> {
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

  editPost(data: EditPostRequest): Observable<PostResponse> {
    return this.http.put<PostResponse>(`${this.Url}/edit`, data);
  }

  getUserPosts(profileId: string): Observable<PostResponse[]> {
    let params = new HttpParams().set('profileId', profileId)
    return this.http.get<PostResponse[]>(`${this.Url}/user`, { params });
  }

  getPost(postId: string): Observable<PostResponse> {
    return this.http.get<PostResponse>(`${this.Url}/${postId}`)
  }

  deletePost(id: string) {
    return this.http.delete(`${this.Url}/${id}`);
  }

  getAllPosts(userId: string): Observable<PostResponse[]> {
    let params = new HttpParams().set('userId', userId)
    return this.http.get<PostResponse[]>(`${this.Url}/get-all`, { params });
  }

  searchPosts(keyword: string): Observable<PostResponse[]> {
    let params = new HttpParams().set('keyword', keyword)
    return this.http.get<PostResponse[]>(`${this.Url}/search-posts`, { params });
  }

  getTrendingPosts(): Observable<PostResponse[]> {
    return this.http.get<PostResponse[]>(`${this.Url}/trending-posts`);
  }

  getSharesCount(postId: string): Observable<number> {
    let params = new HttpParams().set('postId', postId)
    return this.http.get<number>(`${this.Url}/shares-count`, { params });
  }

  getLikesCount(postId: string): Observable<number> {
    let params = new HttpParams().set('postId', postId)
    return this.http.get<number>(`${this.Url}/likes-count`, { params });
  }

  hidePost(postId: string) {
    let params = new HttpParams().set('postId', postId)
    return this.http.get(`${this.Url}/hide-post`, { params });
  }


}
