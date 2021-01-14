import { HttpEvent, HttpHandler, HttpHeaders, HttpInterceptor, HttpParams, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';
import { exhaustMap, take } from 'rxjs/operators';
import { CookieService } from 'ngx-cookie-service';


@Injectable({
  providedIn: 'root'
})
export class AuthInterceptorService implements HttpInterceptor {
  constructor(private authService: AuthService, private cookieService: CookieService) { }


  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return this.authService.user
      .pipe(
        take(1),
        exhaustMap(user => {
          if (!user) {
            return next.handle(req);
          }

          const token = this.cookieService.get('token');
          const headersWithToken = new HttpHeaders().set('Authorization', `Bearer ${token}`);
          const modifiedReq = req.clone({ headers: headersWithToken });

          return next.handle(modifiedReq);
        })
      );
  }
}
