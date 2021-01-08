import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { LoginResponseDTO, RegisterResponseDTO } from './Entities/Interfaces/auth-response';
import { CurrentUser, UserToLoginDTO, UserToRegisterDTO } from './Entities/Models/user.model';
import { CookieService } from 'ngx-cookie-service';
import { Router } from '@angular/router';


const registerUrl = `${environment.baseUrl}/account/register`;
const registerAdminUrl = `${environment.baseUrl}/account/register-admin`;
const loginUrl = `${environment.baseUrl}/account/login`;

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(
    private http: HttpClient,
    private cookieService: CookieService,
    private router: Router
  ) { }


  user = new BehaviorSubject<CurrentUser>(null);
  private tokenExpirationTimer: any;

  register(user: UserToRegisterDTO, registerAsAdmin = false): Observable<RegisterResponseDTO> {
    if (registerAsAdmin) {
      return this.http.post<RegisterResponseDTO>(registerAdminUrl, user);
    }

    return this.http.post<RegisterResponseDTO>(registerUrl, user);
  }

  login(user: UserToLoginDTO): Observable<LoginResponseDTO> {
    return this.http.post<LoginResponseDTO>(loginUrl, user);
  }

  autoLogin(): void {
    const userExists = this.cookieService.check('user');
    const tokenDateExists = this.cookieService.check('tokenExpirationDate');
    if (!userExists || !tokenDateExists) {
      return;
    }

    const user = this.cookieService.get('user');
    const tokenDate = this.cookieService.get('tokenExpirationDate');

    const currentUser: CurrentUser = JSON.parse(user);
    const tokenExpirationDate = new Date(JSON.parse(tokenDate));

    const isTokenLifetimeValid = new Date() < tokenExpirationDate;
    if (isTokenLifetimeValid) {
      this.user.next(currentUser);

      const expirationDuration = tokenExpirationDate.getTime() - new Date().getTime();
      this.autoLogout(expirationDuration);
    }
  }

  logout(): void {
    this.user.next(null);
    this.cookieService.deleteAll();
    this.tokenExpirationTimer = null;
  }

  autoLogout(expirationDuration: number): void {
    if (!this.tokenExpirationTimer) {
      this.tokenExpirationTimer = setTimeout(() => {
        this.logout();
        this.router.navigateByUrl('/login');
      }, expirationDuration);
    }
  }

  handleLoginResponse(loginResponse: LoginResponseDTO): void {
    const currentUser = new CurrentUser(
      loginResponse.userId,
      loginResponse.email,
      loginResponse.isAdmin
    );

    this.user.next(currentUser);

    this.cookieService.set('token', loginResponse.token);
    this.cookieService.set('tokenExpirationDate', JSON.stringify(loginResponse.tokenExpirationDate));
    this.cookieService.set('user', JSON.stringify(currentUser));

    const tokenExpirationDate = new Date(loginResponse.tokenExpirationDate);
    const expirationDuration = tokenExpirationDate.getTime() - new Date().getTime();

    this.autoLogout(expirationDuration);
  }
}



// login()
// Can't access the cookies headers, because of http and not https?

    // return this.http.post<any>(loginUrl, user, { observe: 'response' })
    //   .subscribe(res => {
    //     res.headers.keys()
    //       .map((key) =>
    //       console.log(`${key}: ${res.headers.get(key)}`));
    //   });
