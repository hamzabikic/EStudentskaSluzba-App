import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";
import {Observable} from "rxjs";
import {Injectable} from "@angular/core";
import {MyAuthService} from "./MyAuthService";

@Injectable()
export class MyHttpInterceptor implements HttpInterceptor {
  constructor(private authService : MyAuthService) {

  }
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        var clone_header = req.headers.set("my-token",this.authService.getToken() );
        var clone = req.clone({
          headers: clone_header
        });
        return next.handle(clone);
  }

}
