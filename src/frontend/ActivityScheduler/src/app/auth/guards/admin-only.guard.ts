import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { AuthService } from '../auth.service';


@Injectable({
  providedIn: 'root'
})
export class AdminOnlyGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) { }


  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot):
    Observable<boolean | UrlTree> |
    Promise<boolean | UrlTree> |
    boolean |
    UrlTree
  {
    return this.authService.user
      .pipe(
        take(1),
        map(user => {
          if (!user || !user.isAdmin) {
            return this.router.createUrlTree(['/activities']);
          }

          return true;
        })
      );
  }
}
