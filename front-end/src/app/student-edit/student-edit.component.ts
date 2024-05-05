import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {HttpClient, HttpClientModule} from "@angular/common/http";
import {MojConfig} from "../MojConfig";

@Component({
  selector: 'app-student-edit',
  templateUrl: './student-edit.component.html',
  styleUrls: ['./student-edit.component.css']
})
export class StudentEditComponent implements OnInit {
  id =0;
  smjerovi:any;
  opstine:any;
  student:any;
  putanja:string ="";
  ucitanaSlika = false;
  datumRodjenja ="";
  moguce_slanje = true;

  constructor(private route: ActivatedRoute, private http: HttpClient, private router:Router) { }

  async ngOnInit() {
    this.id = this.route.snapshot.params["id"];
    if(this.route.snapshot.url[1].toString() == "edit") {
    }
    else {
      let inputi = document.querySelectorAll("input");
      let buttons = document.querySelectorAll("button");
      let selects = document.querySelectorAll("select");
      let niz2 = Array.from(buttons);
      let niz = Array.from(inputi);
      for (let x of niz) {
        x.disabled = true;
      }
      for(let x of niz2) {
        x.disabled = true;
      }
      let niz3 = Array.from(selects);
      for(let x of niz3) {
        x.disabled = true;
      }
    }

    await this.ucitajSmjerove();
    await this.ucitajOpstine();
    await this.ucitajStudenta();
    this.putanja= MojConfig.adresa_servera+"/Image/getImageById?id="+this.id;
  }
  async ucitajSmjerove() {
    this.smjerovi = await this.http.get(MojConfig.adresa_servera+"/Student/getSmjerovi").toPromise();
  }
  async ucitajOpstine() {
    this.opstine = await this.http.get(MojConfig.adresa_servera +"/Opstina/getOpstine").toPromise();
  }
  async ucitajStudenta() {
    this.student = await this.http.get(MojConfig.adresa_servera+"/Student/getById?id=" + this.id).toPromise();
    this.datumRodjenja= this.student.datumRodjenja.split('T')[0];
  }
  async sacuvajStudenta() {
    this.student.datumRodjenja = this.datumRodjenja;
    let res =await this.http.put
    (MojConfig.adresa_servera +"/Student/updateStudent?id="+this.id, this.student).toPromise();
    // @ts-ignore
    if(res.dodan) {
      if(this.ucitanaSlika) {
      await this.posaljiSliku();
      }
      alert("Uspjesno editovanje studenta!");
      this.router.navigate(["/studenti"]);
      return;
    }
    // @ts-ignore
    alert(res.greska);
  }
  ucitajSliku() {
     let reader = new FileReader();
     // @ts-ignore
     let slika = document.getElementById("slika-file").files[0]
    if(slika) {
      reader.onload = () => {
         this.putanja = reader.result!.toString();
         this.ucitanaSlika = true;
      }
      reader.readAsDataURL(slika);
    }
    else {
       this.ucitanaSlika= false;
       this.putanja = "";
    }
  }
  async novaLozinka() {
    this.moguce_slanje=false;
    let res =
      await this.http.patch(MojConfig.adresa_servera +"/Korisnik/generisiNovuLozinku?id="+this.id, {}).toPromise();
    if(res) {
      alert("Uspjesno generisana nova lozinka!");
      this.moguce_slanje=true;
      return;
    }
    alert("Neuspjesno generisanje nove lozinke!");
    this.moguce_slanje=true;
  }
  async posaljiSliku() {
    let obj = {
      studentId: this.id,
      base64string: this.putanja
    }
    let res =
      await this.http.post(MojConfig.adresa_servera+"/Image/dodajSliku", obj).toPromise();
    if(res){
      alert("Uspjesno dodana slika!");
      return;
    }
    alert("Neuspjesno dodavanje slike!");
  }
}
