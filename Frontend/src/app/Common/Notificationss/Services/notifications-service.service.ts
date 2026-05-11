import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../../../Environments/environment';
import { NotificationResponse } from '../../../../Models/Notifications/Responses/NotificationResponse';

@Injectable({
  providedIn: 'root'
})
export class NotificationsServiceService {

  private readonly apiUrl = `${environment.apiUrl}Notifications`;
  private hubConnection!: signalR.HubConnection;
  private notificationsSubject = new BehaviorSubject<NotificationResponse[]>([]);
  public notifications$ = this.notificationsSubject.asObservable();
  private unreadCountSubject = new BehaviorSubject<number>(0);
  public unreadCount$ = this.unreadCountSubject.asObservable();

  constructor(private http: HttpClient) {}

  async startConnection(token: string): Promise<void> {
    this.hubConnection = new signalR.HubConnectionBuilder().withUrl(`${environment.hubUrl}notificationHub`, {
        accessTokenFactory: () => token,
      }).withAutomaticReconnect().build();

    await this.hubConnection.start();

    this.hubConnection.on('ReceiveNotification', (notification: NotificationResponse) => {
      const current = this.notificationsSubject.value;
      this.notificationsSubject.next([notification, ...current]);
      this.unreadCountSubject.next(this.unreadCountSubject.value + 1);
    });
  }

  loadAll(profileId: string): void {
    this.http.get<NotificationResponse[]>(`${this.apiUrl}?profileId=${profileId}`)
      .subscribe(n => {
        this.notificationsSubject.next(n);
        this.unreadCountSubject.next(n.filter(x => !x.isRead).length);
      });
  }

  getUnread(profileId: string): Observable<NotificationResponse[]> {
    return this.http.get<NotificationResponse[]>(`${this.apiUrl}/unread?profileId=${profileId}`);
  }

  getByType(profileId: string, type: string): Observable<NotificationResponse[]> {
    return this.http.get<NotificationResponse[]>(`${this.apiUrl}/by-type?profileId=${profileId}&type=${type}`);
  }

  markAsRead(notificationId: string): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${notificationId}/read`, {});
  }

  markAllAsRead(profileId: string): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/read-all?profileId=${profileId}`, {});
  }
}
