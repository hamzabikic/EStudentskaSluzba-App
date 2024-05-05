import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree} from "@angular/router";
import {Observable} from "rxjs";
import {MyAuthService} from "./MyAuthService";
import {Injectable} from "@angular/core";
@Injectable()

export class ReferentProfilCanActivate implements CanActivate {
  constructor(private auth:MyAuthService, private router: Router) {
  }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    if(this.auth.jeLogiran() && this.auth.isReferent()) {
      return true;
    }
    this.router.navigate([""]);
    return false;
  }

}
