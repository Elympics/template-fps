variable "cloudflare_zone_id" {
  description = "cloudflare zone id"
}

variable "cloudflare_api_token" {
  description = "cloudflare api token"
}

variable "deploy_ip" {
  description = "ip address of stg instance"
}

variable "deploy_domain" {
  description = "full domain to which deploy is made"
  default = "template-fps.elympics.cc"
}
