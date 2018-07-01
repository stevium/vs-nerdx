# VsNerdX
[NERDTree](https://github.com/scrooloose/nerdtree) inspired Solution Explorer for Visual Studio. It integrates Vim-like bindings for tree navigation and manipulation into Visual Studio's hierarchy windows.

# Installation
Simplest way to install is through the **Extension Manager** or the **[Visual Studio Marketplace](https://marketplace.visualstudio.com/items?itemName=mstevius.vs-nerdx-solution-explorer)**.
Or if building from source, use the resulting `VsNerdX.vsix`.

Currently supported Visual Studio version is 2017.

# Supported Commands
##### Directory node mappings
* `o` - open & close node
* `x` - close parent of node
##### Tree navigation mappings
* `P` - go to root
* `p` - go to parent
* `j` -  go to next sibling
* `k` - go to prev sibling
* `gg` - go to top
* `G` -  go to bottom
##### Other mappings
* `/` - Enter Find Mode - stops all processing of keys with the exception of Esc
* `Esc` - Exit Find Mode - resumes handling of navigation keys

# Reporting Problems
Please use the [issue tracker](https://github.com/mstevius/vs-nerdx/issues).
