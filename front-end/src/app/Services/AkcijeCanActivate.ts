import {Injectable} from "@angular/core";
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree} from "@angular/router";
import {Observable} from "rxjs";
import {MyAuthService} from "./MyAuthService";

@Injectable()
export class AkcijeCanActivate implements CanActivate {
    constructor (private myAuth : MyAuthService, private router: Router) {

    }
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
         if(this.myAuth.jeLogiran() && this.myAuth.isReferent() ) {
           return true;
         }
         if(this.myAuth.jeLogiran()){
         this.router.navigate([""]);
         return false;
         }
      this.router.navigate(["/login"]);
      return false;
    }

}
