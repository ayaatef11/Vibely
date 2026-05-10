import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../Environments/environment';
import { StartSharePostRequest } from '../../../Models/Posts/Requests/StartSharePostRequest';
import { RevokeSharePostRequest } from '../../../Models/Posts/Requests/RevokeSharePostRequest';

@Injectable({
  providedIn: 'root'
})
export class SharePostServiceService {
  private Url = environment.apiUrl + 'Share'
  constructor(private http: HttpClient) { }

  startSharePost(data: StartSharePostRequest) {
    return this.http.post(`${this.Url}/Start`, data, { responseType: 'text' });
  }

  revokeSharePost(data: RevokeSharePostRequest) {
    return this.http.post(`${this.Url}/revoke`, data);
  }

  openShareLink(link: string) {
    return this.http.get(`${this.Url}/share/${link}`);
  }
}
