import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import {HttpClient, provideHttpClient, withFetch, withInterceptors, withInterceptorsFromDi} from '@angular/common/http';
import { provideToastr } from 'ngx-toastr';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import {authenticationInterceptor} from '../Interceptors/Authentication/authentication.interceptor';
import { Observable } from 'rxjs';

export class CustomTranslateLoader implements TranslateLoader {
  constructor(private http: HttpClient) {}

  getTranslation(lang: string): Observable<any> {
    return this.http.get(`/assets/i18n/${lang}.json`);
  }
}

export function HttpLoaderFactory(http: HttpClient) {
  return new CustomTranslateLoader(http);
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(
    withFetch(),
    withInterceptors([authenticationInterceptor]),
    withInterceptorsFromDi()
    ),
    provideZoneChangeDetection({ eventCoalescing: true }),    
    //  AuthService,
          {
      provide: 'SocialAuthServiceConfig',
      useValue: {
      autoLogin: false, 
    }
  }
,
     provideClientHydration(),
     importProvidersFrom(
    // NgxSpinnerModule.forRoot({ type: 'ball-scale-multiple' }),
      //  ToastrModule.forRoot({
      //   timeOut: 3000,
      //   positionClass: 'toast-bottom-right',
      //   preventDuplicates: true,
      // })
      TranslateModule.forRoot({
        fallbackLang: 'en',
        loader: {
          provide: TranslateLoader,
          useFactory: HttpLoaderFactory,
          deps: [HttpClient]
        }
      })
    ),
    provideAnimationsAsync(),
    provideToastr()
    ]
};
