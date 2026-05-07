import { Routes } from '@angular/router';
import { SignupComponent } from './Auth/signup/Component/signup.component';
import { LoginComponent } from './Auth/login/Component/login.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { CreatePostComponent } from './Post/create-post/create-post.component';
import { UserPostComponent } from './Post/user-post/user-post.component';
import { AllComponent } from './Post/SavedPosts/All/All.component';
import { SavedReelsComponent } from './Post/SavedPosts/reels/Savedreels.component';
import { AllNotificationsComponent } from './Common/Notificationss/AllNotifications/AllNotifications.component';
import { UnReadNotificationsComponent } from './Common/Notificationss/un-read-notifications/un-read-notifications.component';
import { CommentsNotificationsComponent } from './Common/Notificationss/comments-notifications/comments-notifications.component';
import { LikesNotificationsComponent } from './Common/Notificationss/likes-notifications/likes-notifications.component';
import { FollowedFriendsComponent } from './Friends/Followed-Friends/Followed-Friends.component';
import { FollowingFriendsComponent } from './Friends/following-friends/following-friends.component';
import { SearchComponent } from './Common/search/search.component';
import { SearchPeopleComponent } from './Common/search/search-people/search-people.component';
import { SearchPostsComponent } from './Common/search/search-posts/search-posts.component';
import { SearchTagsComponent } from './Common/search/search-tags/search-tags.component';
import { EditProfileComponent } from './UserProfile/edit-profile/edit-profile.component';
import { ProfileComponent } from './UserProfile/profile/profile.component';
import { FriendChatComponent } from './Chatting/friend-chat/friend-chat.component';
import { ChatsComponent } from './Chatting/chats/chats.component';
import { NotFoundComponent } from './Common/not-found/not-found.component';
import { SettingsComponent } from './Common/settings/settings.component';
import { AccountComponent } from './Common/settings/account/account.component';
import { PrivacyAndSecurityComponent } from './Common/settings/privacy-and-security/privacy-and-security.component';
import { PreferencesComponent } from './Common/settings/preferences/preferences.component';
import { SettingsProfileComponent } from './Common/settings/settings-profile/settings-profile.component';
import { NotificationsHeaderComponent } from './Common/Notificationss/notifications-header/notifications-header.component';
import { SidebarComponent } from './Common/sidebar/sidebar.component';
import { ReelsComponent } from './Common/reels/reels.component';
import { CodeVerificationComponent } from './Auth/code-verification/code-verification.component';
import { ForgetPasswordComponent } from './Auth/forget-password/forget-password.component';

export const routes: Routes = [
  { path: 'signup', title: 'Sign Up', component: SignupComponent },
  { path: 'login', title: 'Sign in', component: LoginComponent },
  { path: 'verify-code', title: 'Code Verification', component: CodeVerificationComponent },
  { path: 'forget-password', title: 'Forget Password', component: ForgetPasswordComponent },
  { path: 'create/post', title: 'Create Post', component: CreatePostComponent },

  {
    path: 'saved-posts', title: 'Saved Posts', component: AllComponent,
    children: [

      { path: 'reels', title: 'Reels', component: SavedReelsComponent },
    ]
  },

  {
    path: 'home', title: 'Main', component: SidebarComponent,
    children: [
      {
        path: '', component: DashboardComponent
      },
      { path: 'user/post/:id', title: 'Post', component: UserPostComponent },
      { path: 'chat/view', component: ChatsComponent },
      { path: 'chat/friend', component: FriendChatComponent },

      {
        path: 'notifications', title: 'Notifications', component: NotificationsHeaderComponent,
        children: [
          { path: 'all', component: AllNotificationsComponent },
          { path: 'unread', component: UnReadNotificationsComponent },
          { path: 'comments', component: CommentsNotificationsComponent },
          { path: 'likes', component: LikesNotificationsComponent },
          { path: '', redirectTo: 'all', pathMatch: 'full' }
        ]
      },
      { path: 'profile/edit', component: EditProfileComponent },
      { path: 'profile/:id', component: ProfileComponent },
      { path: '', redirectTo: 'notifications', pathMatch: 'full' },
      { path: 'reels', component: ReelsComponent },
     
      { path: 'search/all', title: 'Search', component: SearchComponent },
      { path: 'search/people', component: SearchPeopleComponent },
      { path: 'search/posts', component: SearchPostsComponent },
      { path: 'search/tags', component: SearchTagsComponent },

    ]
  },
  { path: 'friends/followers', title: 'Followers', component: FollowedFriendsComponent },
  { path: 'friends/following', title: 'Following', component: FollowingFriendsComponent },


  {
    path: 'settings', component: SettingsComponent,
    children: [
      { path: '', redirectTo: 'account', pathMatch: 'full' },
      { path: 'account', component: AccountComponent },
      { path: 'privacy', component: PrivacyAndSecurityComponent },
      { path: 'preferences', component: PreferencesComponent },
      { path: 'profile', component: SettingsProfileComponent }
    ]
  },
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'not-found', component: NotFoundComponent },
  { path: '**', redirectTo: 'home' }

];
