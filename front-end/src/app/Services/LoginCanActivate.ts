import {Injectable} from "@angular/core";
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree} from "@angular/router";
import {Observable} from "rxjs";
import {MyAuthService} from "./MyAuthService";

@Injectable()
export class LoginCanActivate implements CanActivate {
  constructor (private myAuth : MyAuthService, private router: Router) {

  }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    if(!this.myAuth.jeLogiran()) {
      return true;
    }
    if(this.myAuth.isReferent()){
      this.router.navigate(["/referent/profil"]);
    }
    else if(this.myAuth.isNastavnik()) {
      this.router.navigate(["/profesor/profil"]);
    }
    else {
      this.router.navigate(["/student/profil"]);
    }
    return false;
  }

}
