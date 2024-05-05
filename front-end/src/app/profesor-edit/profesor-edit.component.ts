import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {HttpClient} from "@angular/common/http";
import {MojConfig} from "../MojConfig";

@Component({
  selector: 'app-profesor-edit',
  templateUrl: './profesor-edit.component.html',
  styleUrls: ['./profesor-edit.component.css']
})
export class ProfesorEditComponent implements OnInit {

  id =0;
  smjerovi:any;
  opstine:any;
  profesor:any;
  putanja:string ="";
  ucitanaSlika = false;
  datumZaposlenja ="";
  datumRodjenja="";
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
    await this.ucitajProfesora();
    this.putanja= MojConfig.adresa_servera+"/Image/getImageById?id="+this.id;
  }
  async ucitajSmjerove() {
    this.smjerovi = await this.http.get(MojConfig.adresa_servera+"/Student/getSmjerovi").toPromise();
  }
  async ucitajOpstine() {
    this.opstine = await this.http.get(MojConfig.adresa_servera +"/Opstina/getOpstine").toPromise();
  }
  async ucitajProfesora() {
    this.profesor = await this.http.get(MojConfig.adresa_servera+"/Nastavnik/getNastavnikById?id=" + this.id).toPromise();
    this.datumRodjenja = this.profesor.datumRodjenja.split('T')[0];
    this.datumZaposlenja = this.profesor.datumZaposlenja.split('T')[0];
  }
  async sacuvajProfesora() {
    this.profesor.datumRodjenja = this.datumRodjenja;
    this.profesor.datumZaposlenja = this.datumZaposlenja;
    let res =await this.http.put
    (MojConfig.adresa_servera +"/Nastavnik/editNastavnik?id="+this.id, this.profesor).toPromise();
    // @ts-ignore
    if(res.dodan) {
      if(this.ucitanaSlika) {
        await this.posaljiSliku();
      }
      alert("Uspjesno editovanje profesora!");
      this.router.navigate(["/profesori"]);
      return;
    }
    //@ts-ignore
    alert(res.greska);
  }
  async novaLozinka() {
    this.moguce_slanje=false;
    let res =
      await this.http.patch(MojConfig.adresa_servera +"/Korisnik/generisiNovuLozinku?id="+this.id, {}).toPromise();
    if(res) {
      alert("Uspjesno generisana nova lozinka!");
      this.moguce_slanje= true;
      return;
    }
    alert("Neuspjesno generisanje nove lozinke!");
    this.moguce_slanje= true;
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
