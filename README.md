# UnityRO

![GitHub issues](https://img.shields.io/github/issues/guilhermelhr/unityro)
![Repo size](https://img.shields.io/github/repo-size/guilhermelhr/unityro)
[![License: AGPL v3](https://img.shields.io/badge/License-AGPL%20v3-blue.svg)](LICENSE)
[![GitHub release](https://img.shields.io/github/release/guilhermelhr/unityro.svg)](https://github.com/guilhermelhr/unityro/releases/)
[![Discord](https://img.shields.io/discord/780225096463286273.svg?label=&logo=discord&logoColor=ffffff&color=7389D8&labelColor=6A7EC2)](https://discord.gg/YZjGanTNb5)

>Ragnarok Online was released in 2002, thus its engine is severely outdated.
>This project aims to port some of its functionalities to a modern engine, Unity.

Join discussions at our [Discord](https://discord.gg/YZjGanTNb5).

## Motivation

### Why this instead of contributing to roBrowser or any other?

We at UnityRO recognize the power WebGL has over a "Jack of all trades" solution like Unity. However, it is a stable multi-platform solution with lots of plug and play features, specially physics (not that RO has much), which makes things much easier to handle, maintain and future-proof.

## How to contribute?

At this moment we're still under a heavy amount of initial work, however we try to follow the Git-Flow pattern where
* `master` is a point of release once `develop` reach a milestone
* `develop` is where all PRs get merged
* `bug/*` or `feature/*` for branches diverging from `develop`

We would also like to encourage the usage of [Issues](https://github.com/guilhermelhr/unityro/issues) and of [Projects](https://github.com/guilhermelhr/unityro/projects/1) so we can have a better sigthseeing of the current state of work.
 
### Branch Naming
As for the branch naming, we encourage of having every branch linked to a specific issue following the pattern:

`feature/<issue number>_<very brief description>` eg: `feature/32_update-packets-to-2020`

## Getting Started

We're creating a few pieces of documentation, feel free to check them at [Wiki](https://github.com/guilhermelhr/unityro/wiki/Getting-Started).

## Current Features
- This is the current state we're at:

<a href="http://www.youtube.com/watch?feature=player_embedded&v=3Q76pVG46tA" target="_blank"><img src="http://img.youtube.com/vi/3Q76pVG46tA/0.jpg" alt="UnityRO" width="300" border="10" /></a>

- Runtime threaded GRF loading

<img src="https://thumbs.gfycat.com/PerkyEnlightenedDuck-size_restricted.gif" width="300">

- Realtime lighting and shadows

<img src="https://thumbs.gfycat.com/LargeWarlikeFinch-size_restricted.gif" width="300">

- Baked lightmaps

<img src="https://i.imgur.com/i49WTg1.jpg" width="300">

- Model animations

<img src="https://thumbs.gfycat.com/SilkyWaryArkshell-size_restricted.gif" width="300">

- Texture transparency

<img src="https://thumbs.gfycat.com/AnnualThatBison-size_restricted.gif" width="300">

- Sky backdrops

<img src="https://thumbs.gfycat.com/SpiritedLoneCormorant-size_restricted.gif" width="300">

- 3D SFX

<img src="https://i.imgur.com/ZEO6FiO.jpg" width="300">

## How to use

Download executable or Unity package [here](https://github.com/guilhermelhr/unityro/releases/).
Edit config.txt to point to your GRF file.

## License

This project is licensed under the GNU Affero General Public License v3.0 - see the [LICENSE](LICENSE) file for details.
