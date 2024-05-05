import {Injectable} from "@angular/core";

@Injectable()
export class MyAuthService {
    jeLogiran():boolean {
      if(localStorage.getItem("my-token")){
        return true;
      }

      return false;
    }
    getId ():number {
      if (this.jeLogiran()) {
        let obj = JSON.parse(localStorage.getItem("my-token")!);
        return obj.prijava.korisnik.id;
      }
      return 0;
    }
    getToken():string {
      if (this.jeLogiran()) {
        let obj = JSON.parse(localStorage.getItem("my-token")!);
        return obj.prijava.token;
      }
      return "";
    }
    getImePrezime () :string {
      if(this.jeLogiran()) {
        let obj = JSON.parse(localStorage.getItem("my-token")!);
        return obj.prijava.korisnik.ime +" " + obj.prijava.korisnik.prezime;
      }
      return "";
    }
    isReferent() {
      if(this.jeLogiran()) {
        let obj = JSON.parse(localStorage.getItem("my-token")!);
        if(obj.prijava.korisnik.isReferent) {
          return true;
        }
        return false;
      }
      return false;
    }
    isStudent() {
      if(this.jeLogiran()) {
        let obj = JSON.parse(localStorage.getItem("my-token")!);
        if(obj.prijava.korisnik.isStudent) {
          return true;
        }
        return false;
      }
      return false;
    }
    isNastavnik() {
      if(this.jeLogiran()) {
        let obj = JSON.parse(localStorage.getItem("my-token")!);
        if(obj.prijava.korisnik.isNastavnik) {
          return true;
        }
        return false;
      }
      return false;
    }
}
